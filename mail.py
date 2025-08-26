import json
from pathlib import Path

PATH = Path("~/.config/updateChecker/mail.json").expanduser()

receiver_name = input("Receiver name: ")
receiver_emal = input("Receiver email: ")
sender_email = input("Sender email: ")
sender_password = input("Sender password: ")
host = input("Mail provider: ")
port = input("Provider port: ")

info = {
    "receiver_name": receiver_name,
    "receiver_email": receiver_emal,
    "sender_email": sender_email,
    "sender_password": sender_password,
    "host": host,
    "port": port,
}

info = json.dumps(info, indent=4)

with open(PATH, "w") as file:
    _ = file.write(info)
