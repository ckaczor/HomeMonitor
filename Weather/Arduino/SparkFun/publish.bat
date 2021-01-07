SET REMOTE=ckaczor@172.23.10.6

plink %REMOTE% mkdir -p Weather/Arduino

pscp -v -r makefile Weather.ino %REMOTE%:Weather/Arduino
