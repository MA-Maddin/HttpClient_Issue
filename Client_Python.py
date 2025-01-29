import requests

url = "http://localhost:5000/upload"
file_path = "C:\\path-to-100MB-file"

headers = {
    'Expect': '100-continue'
}

# Prepare the file to send
with open(file_path, 'rb') as f:
    files = {'file': (file_path, f)}
    response = requests.post(url, files=files)

# Print the response
print(response.status_code)
print(response.text)
