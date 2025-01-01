#!/bin/bash

echo "Beginning Refresh Script" 

cd ../../FantasyFootballScraper || exit 1 

source ./venv/bin/activate 
python3 main.py
deactivate