# IHX-Integraters

Integration Snippets for IHX Services

## How to use

1. Clone the repository

### NodeJS

> Navigate to the NodeJS folder

```bash
cd IHX-Integraters/<Service_Name>/NodeJS
```

> Run the following command to install the dependencies

```bash
npm install
```

> Fill in the required credentials in the code and Run the code using the following command

```bash
node sample_request.js
```

### Python

> Navigate to the Python folder

```bash
cd IHX-Integraters/<Service_Name>/Python
```

> Run the following command to install the dependencies

```bash
pip install -r requirements.txt
```

> Fill in the required credentials in the code and Run the code using the following command

```bash
python sample_request.py
```

### C Sharp

> Navigate to the Python folder

```bash
cd IHX-Integraters/<Service_Name>/C-Sharp
```

> Run the following commands to create a project (You already need to have dotnet installed)

```bash
dotnet new console
mv sample_request.cs Program.cs
dotnet add package Newtonsoft.Json
```

> Fill in the required credentials in the code and Run the code using the following command

```bash
dotnet build
dotnet run
```