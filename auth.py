#! ./.venv/bin/python3

import requests
import getpass
import json
from pathlib import Path
from pprint import pprint

URL = "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token"
PATH = Path("~/.config/updateChecker/credentials.json").expanduser()
ACCESSS = "access_token"
REFRESH = "refresh_token"


def main():
    username = input("Type your username: ").strip()
    password = getpass.getpass("Type your passowrd: ").strip()
    client_id = input("Type your client id: ").strip()
    client_secret = getpass.getpass("Type your client secret: ").strip()

    info = {
        "grant_type": "password",
        "username": username,
        "password": password,
        "client_id": client_id,
        "client_secret": client_secret,
    }

    res = requests.post(URL, info)

    if res.status_code != 200:
        pprint(res.reason)
        pprint(res.content)
        return

    res = res.json()
    acc = res[ACCESSS]
    ref = res[REFRESH]

    creds = {
        ACCESSS: acc,
        REFRESH: ref,
        "client_id": client_id,
        "client_secret": client_secret,
    }

    with open(PATH, "w") as file:
        file.write(json.dumps(creds, indent=4))


if __name__ == "__main__":
    main()
