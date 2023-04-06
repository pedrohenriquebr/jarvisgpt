#!/usr/bin/env bash

uninstall()
{
    # Path to JarvisGPT source
    local JARVISGPT_CLI_PATH="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && cd .. && pwd )"
    local BASH_RC_FILE="$HOME/.jarvisgptrc"
    # Remove the plugin loaded by .bashrc
    rm -f $BASH_RC_FILE
    # Remove key binding (works only for sourced script calls)
    if [ $SOURCED -eq 1 ]; then
        bind -r "\C-g"
    fi

    echo "Jarvis GPT has been removed."
}

# Detect if the script is sourced
(return 0 2>/dev/null) && SOURCED=1 || SOURCED=0

uninstall
sudo rm /usr/bin/jarvis
sudo systemctl stop jarvisgpt.service
sudo systemctl disable jarvisgpt.service
sudo rm /etc/systemd/system/jarvisgpt.service
sudo systemctl daemon-reload

unset SOURCED