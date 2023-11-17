FROM langchain/langchain as builder

RUN apt-get update && apt-get install --no-install-recommends -y \
    build-essential \
    curl \
    software-properties-common \
    && rm -rf /var/lib/apt/lists/*

RUN python3 -m venv /venv --system-site-packages

ENV PATH=/venv/bin:$PATH

WORKDIR /app

COPY requirements.txt .

RUN pip install -r requirements.txt

FROM langchain/langchain

COPY --from=builder /venv /venv

ENV PATH=/venv/bin:$PATH

COPY src/ .
COPY --from=config .env .

ARG PORT=8100
ENV PORT=${PORT}

ARG HOST=0.0.0.0
ENV HOST=${HOST}

EXPOSE ${PORT}

HEALTHCHECK CMD curl --fail http://localhost:${PORT}

ENTRYPOINT streamlit run main.py --server.address ${HOST} --server.port ${PORT}