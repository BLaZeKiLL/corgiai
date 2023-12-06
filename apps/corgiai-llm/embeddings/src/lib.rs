use anyhow::Result;
use spin_sdk::{
    http::{Request, Response},
    http_component,
    llm::{generate_embeddings, EmbeddingModel},
};

use serde::{Deserialize, Serialize};

#[derive(Deserialize)]
pub struct OllamaEmbeddingRequest {
    pub model: String,
    pub prompt: String,
}

#[derive(Serialize)]
pub struct OllamaEmbeddingResponse {
    pub embedding: Vec<f32>
}

/// A simple Spin HTTP component.
#[http_component]
fn handle_embeddings(req: Request) -> Result<Response> {
    println!("Handling request to {:?}", req.header("spin-full-url"));
    
    let request: OllamaEmbeddingRequest = serde_json::from_slice(req.body())?;

    let result = generate_embeddings(
        EmbeddingModel::AllMiniLmL6V2, &[request.prompt]
    );

    let response = OllamaEmbeddingResponse { embedding: result?.embeddings.remove(0) };

    send_ok_response(200, response)
}

fn send_ok_response(code: u16, resp: OllamaEmbeddingResponse) -> Result<Response> {
    Ok(spin_sdk::http::Response::builder()
        .status(code)
        .header("content-type", "application/json")
        .body(Some(serde_json::to_string(&resp)?))
        .build()
    )
}