################################################
## *** Codex CLI plugin function for Bash *** ##
##         loaded by $HOME/.codexclirc        ##
################################################

create_completion()
{
    # Get the text typed until now
    text=${READLINE_LINE}
    completion=$(echo -n "$text" | $JARVISGPT_CLI_PATH/cli-plugin/main.py)
    # Add completion to the current buffer
    READLINE_LINE="${text}${completion}"
    # Put the cursor at the end of the line
    READLINE_POINT=${#READLINE_LINE}
}