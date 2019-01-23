::
@echo OFF
title SaltyEmu - Initialization Script

echo "Pulling docker images"
call .\scripts\management\pull_dockers.bat

echo "Starting database..."
call .\scripts\management\start_bdd.bat

echo "Starting session..."
call .\scripts\management\start_session.bat

echo "Starting MQTT Broker..."
call .\scripts\management\start_mqtt_broker.bat

echo "Building Login Docker..."
call .\scripts\build\build_docker_Login.bat

echo "Starting Login Server under Docker..."
call .\scripts\management\start_login.bat
pause