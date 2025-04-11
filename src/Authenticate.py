import requests
import json
from pathlib import PosixPath

FILE_PATH = PosixPath("~/.config/updateChecker/credentials.json").expanduser()
    

URL = "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token"

HEADERS = {
    "Content-Type": "application/x-www-form-urlencoded"
}

data = {
    "grant_type": "password",
    "username": "",
    "password": ""
}


if __name__ == "__main__":
    with open(FILE_PATH, "r") as file:
        fileData = json.load(file)
        data["client_id"] = fileData["ClientId"]
        data["client_secret"] = fileData["ClientSecret"]
    
    print(fileData)
    
    response = requests.post(URL, data=data, headers=HEADERS)
    print(response)
    if response.status_code != 200:
        print(response.reason)
        print(response.content)
    else:
        jsonResponse = response.json()
        fileData["AccessToken"] = jsonResponse["access_token"]
        fileData["RefreshToken"] = jsonResponse["refresh_token"]
        fileString = json.dumps(fileData)
        with open(FILE_PATH, "w") as file:
                file.write(fileString)