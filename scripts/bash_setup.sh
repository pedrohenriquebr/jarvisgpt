#!/usr/bin/env bash

configureBash()
{
    echo "*** Configuring bash [$BASH_RC_FILE] ***"
    echo -n > $HOME/.jarvisgptrc
    echo "export JARVISGPT_CLI_PATH=\"${JARVISGPT_CLI_PATH}\"" >> $BASH_RC_FILE
    echo 'source "$JARVISGPT_CLI_PATH/scripts/bash_plugin.sh"' >> $BASH_RC_FILE
    echo "bind -x '\"\C-g\":\"create_completion\"'" >> $BASH_RC_FILE
    if [ $SOURCED -eq 1 ]; then
        echo "*** Testing bash settings [$BASH_RC_FILE] ***"
        source "$BASH_RC_FILE"
    fi
}

configureService(){

sudo touch /etc/systemd/system/jarvisgpt.service

echo -e "[Unit]
Description=JarvisGPT Api
After=network.target

[Service]
Type=simple
User=root
ExecStart=$JARVISGPT_CLI_PATH/scripts/startup-server.sh
Restart=on-failure
RestartSec=5s
[Install]
WantedBy=multi-user.target" | sudo tee /etc/systemd/system/jarvisgpt.service

sudo systemctl daemon-reload
sudo systemctl enable jarvisgpt.service
sudo systemctl start jarvisgpt.service

}


enableApp()
{
    echo "*** Activating application [$HOME/.bashrc] ***"
    # Check if already installed
    if grep -Fq ".jarvisgptrc" $HOME/.bashrc; then
        return 0
    fi
    echo -e "\n# Initialize Codex CLI" >> $HOME/.bashrc
    echo 'if [ -f "$HOME/.jarvisgptrc" ]; then' >> $HOME/.bashrc
    echo '    . "$HOME/.jarvisgptrc"' >> $HOME/.bashrc
    echo 'fi' >> $HOME/.bashrc
}

(return 0 2>/dev/null) && SOURCED=1 || SOURCED=0


JARVISGPT_CLI_PATH="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && cd .. && pwd )"
BASH_RC_FILE="$HOME/.jarvisgptrc"

configureBash
enableApp
configureService

sudo ln -s "$JARVISGPT_CLI_PATH/cli-plugin/jarvis.py" /usr/bin/jarvis
sudo chmod +x /usr/bin/jarvis
