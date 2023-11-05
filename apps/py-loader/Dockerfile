FROM langchain/langchain

WORKDIR /app

RUN apt-get update && apt-get install -y \
    build-essential \
    curl \
    software-properties-common \
    && rm -rf /var/lib/apt/lists/*

COPY requirements.txt .

RUN pip install --upgrade -r requirements.txt

COPY src/ .
COPY --from=config .env .

ARG PORT=8100
ENV PORT=${PORT}

ARG HOST=0.0.0.0
ENV HOST=${HOST}

EXPOSE ${PORT}

HEALTHCHECK CMD curl --fail http://localhost:${PORT}

ENTRYPOINT streamlit run main.py --server.address ${HOST} --server.port ${PORT}