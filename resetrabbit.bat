@echo off
if [%1]==[] goto error

echo executing %1 command...
"c:\Program Files\RabbitMQ Server\rabbitmq_server-3.7.8\sbin\rabbitmqctl.bat" %1
goto end

:error
echo ********************* INSTRUCTIONS *********************
echo * To reset RabbitMQ launch this 3 commands in sequence *
echo * resetrabbit stop_app                                 *
echo * resetrabbit reset                                    *
echo * resetrabbit start_app                                *
echo ********************************************************
:end
