#!/usr/bin/env python3
import sys
import requests
import json
import os
import re
import hashlib
from pathlib import Path

output_extensions = {
    'csharp': '.cs',
    'python3': '.py',
    'python': '.py',
    'javascript': '.js',
    'typescript': '.ts',
    'golang': '.go',
    'html': '.html',
    'sql': '.sql',
    'dart': '.dart',
    'json': '.json',
    'c': '.c',
    'Makefile': ''
}

CACHE_DIR = "~/.jarvis-cache"

api_url = 'http://localhost:3001/api/pseudo'

JARVIS_FOLDER_NAME = '.jarvis'


class FileCacheManager:
    def check(self, hash: str) -> None | str:
        pass

    def save(self, hash: str, content: str) -> None:
        pass


class FileCacheManagerImpl(FileCacheManager):
    def __init__(self):
        self.cache_dir = Path(os.path.expanduser(CACHE_DIR))
        self.cache_dir.mkdir(parents=True, exist_ok=True)

    def check(self, hash: str) -> None | str:
        file_path = self.cache_dir / hash
        if file_path.exists():
            with file_path.open("r") as file:
                return file.read()
        return None

    def save(self, hash: str, content: str) -> None:
        file_path = self.cache_dir / hash
        with file_path.open("w") as file:
            file.write(content)


def read_file_content(file_path: str) -> str:
    with open(file_path, 'r') as file:
        content = file.read()
    return content

def write_file_content(file_path: str, content: str) -> None:
    with open(file_path, 'w') as file:
        file.write(content)

def extract_language(text):
    pattern = r'language:\s*(\w+)'
    match = re.search(pattern, text)
    
    if match:
        language = match.group(1)
        return language
    else:
        return None

def get_filename_and_extension(path):
    # Split the path into head and tail (filename with extension)
    head, tail = os.path.split(path)

    # Split the tail into filename without extension and the extension
    filename_without_extension, extension = os.path.splitext(tail)

    return filename_without_extension, extension

def send_payload(url: str, payload: dict) -> dict:
    response = requests.post(url, json=payload)
    response_json = response.json()
    return response_json


def list_yaml_files(path = '.'):
    folder_path = os.path.join(path, JARVIS_FOLDER_NAME)
    
    if not os.path.exists(folder_path) or not os.path.isdir(folder_path):
        print("There isn't "+ JARVIS_FOLDER_NAME + " folder on path!")
        sys.exit(1)
    
    all_files = os.listdir(folder_path)
    yaml_files = [os.path.join(folder_path, file) for file in all_files if file.endswith('.yml') or file.endswith('.yaml')]
    return yaml_files


def to_sha256(string):
    byte_string = string.encode()
    sha256 = hashlib.sha256(byte_string).hexdigest()
    return sha256


def process_file(file_path: str,output_extensions: dict[str, str], api_url: str) -> None:
    filename, ext = get_filename_and_extension(file_path)
    print(f'Processing {os.path.basename(file_path)}...', end='')

    file_content = read_file_content(file_path)
    detected_target_language = extract_language(file_content)
    file_hash = to_sha256(file_content)

    cache_manager = FileCacheManagerImpl()
    if detected_target_language is None:
        print('\nERROR: You need to specify the language inside of file')
        sys.exit(1)

    if detected_target_language not in output_extensions.keys():
        print(
            f'\nERROR: Language not supported yet! value: \'{detected_target_language}\'')
        sys.exit(1)

    output_target_file_extension = output_extensions[detected_target_language]
    # result = cache_manager.check(file_hash) 
    result = None
    
    if result is not None:
        print('cache hit')
        target_full_path = os.path.join(os.path.split(
            file_path)[0], '..', f'{filename}{output_target_file_extension}')
        write_file_content(target_full_path, result)
        print('OK')
        return
    
    print('cache miss')
    

    payload = {'content': file_content}
    response_json = send_payload(api_url, payload)

    result = response_json.get('result')
    result = result.replace('```', '')
    target_full_path = os.path.join(os.path.split(file_path)[0],'..',f'{filename}{output_target_file_extension}')
    write_file_content(target_full_path, result)
    print('OK')
    cache_manager.save(file_hash, result)

def main():

    if len(sys.argv) == 1:
        print("jarvis build [folder]")
        sys.exit(0)

    command = sys.argv[1]

    if command == 'build':
        dir_path = sys.argv[2] if len(sys.argv) > 2 else '.'  
        files = list_yaml_files(dir_path)
        for file in files:
            process_file(file, output_extensions, api_url)
        sys.exit(0)

    if command  == 'reset':
        requests.post(api_url + '/reset')

    # if file_path[::-1][:5][::-1]!= '.yaml' \
    #     and file_path[::-1][:4][::-1] != '.yml':
    #     print(f'Invalid file! extension detected: \'{ext}\' \nonly yaml files')
    #     sys.exit(1)

    # process_file(file_path)

if __name__ == '__main__':
    main()
