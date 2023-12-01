use spin_sdk::http::{IntoResponse, Request};
use spin_sdk::http_component;
use spin_sdk::llm;

use serde::{Deserialize, Serialize};

#[derive(Deserialize)]
pub struct OllamaGenerateRequest {
    pub model: String,
    pub prompt: String,
    pub stream: bool,
    // options
}

// Theres other stuff also look at - https://github.com/jmorganca/ollama/blob/main/docs/api.md#response-1
#[derive(Serialize)]
pub struct OllamaGenerateResponse
{
    pub model: String,
    pub response: String,
    // context
    pub done: bool,
}

/// A simple Spin HTTP component.
#[http_component]
fn handle_generate(req: Request) -> anyhow::Result<impl IntoResponse> {
    println!("Handling request to {:?}", req.header("spin-full-url"));

    let model = llm::InferencingModel::Llama2Chat;

    

    Ok(http::Response::builder()
        .status(200)
        .header("content-type", "text/plain")
        .body("Hello, Fermyon")?
    )
}
