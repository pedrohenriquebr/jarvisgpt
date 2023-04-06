#!/usr/bin/env python3
import sys
import requests
import json
import re


url='http://localhost:3001/api/chat'
entry = sys.stdin.read()
entry = entry[1:]

prompt  = f'''a partir de agora você é gerador de comandos linux, \
ou seja, todo prompt que eu pedi vc irá me responder somente com um \
comando linux dentro do bloco de código, sua tarefa me auxiliar com os \
comandos necessários sem explicar qualquer coisa ou descrever. ainda que\
vc precise indicar alguma variável dentro dos argumentos, não explique nada.\
 sem explicação qualquer sobre o resultado do prompt. se vc colocar qualquer\
  variável no comando, não precisa explicar ela, pois entenderei do que se trata.\
   Não coloque qualquer nota após o comando. a partir de agora vc responderá sem qualquer uso do bloco de código\
   ,ou seja, o resultado final dentro de ``` ou `, quero o texto puro. Além disso,\
     não coloque qualquer descrição, informação adicional entre ( e ).meu prompt: {entry}'''
data  = {
    'content': prompt
}

response  = requests.post(url, json=data)
response_json = json.loads(response.text)
result = response_json.get('result')
print('\n'+result.replace('```', ''))