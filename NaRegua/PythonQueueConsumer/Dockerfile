# Define a imagem base que será usada
FROM python:3.9

# Crie a pasta /app/logs dentro do container
RUN mkdir -p /app/logs

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia o arquivo requirements.txt para o contêiner
COPY requirements.txt .

# Instala as dependências do projeto a partir do arquivo requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Copia todo o código-fonte do projeto para o contêiner
COPY . .

# Define o comando de entrada para executar o arquivo principal do projeto (por exemplo, main.py)
CMD ["python", "main.py"]
