#!/bin/sh

sed -i -e "s~#API_PREFIX#~$API_PREFIX~g" /usr/share/nginx/html/assets/*
sed -i -e "s~#HOME_ASSISTANT_URL#~$HOME_ASSISTANT_URL~g" /usr/share/nginx/html/assets/*
sed -i -e "s~#HOME_ASSISTANT_TOKEN#~$HOME_ASSISTANT_TOKEN~g" /usr/share/nginx/html/assets/*
sed -i -e "s~#GARAGE_DEVICE#~$GARAGE_DEVICE~g" /usr/share/nginx/html/assets/*
sed -i -e "s~#ALARM_DEVICE#~$ALARM_DEVICE~g" /usr/share/nginx/html/assets/*
sed -i -e "s~#CALENDAR_EMBED_URL#~$CALENDAR_EMBED_URL~g" /usr/share/nginx/html/assets/*
