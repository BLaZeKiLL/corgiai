use anyhow::Result;
use spin_sdk::{
    http::{Request, Response},
    http_component,
    llm::{infer_with_options, InferencingModel, InferencingParams},
};

use serde::{Deserialize, Serialize};

#[derive(Deserialize)]
pub struct OllamaOptions {
    pub num_predict: u32,
    pub temperature: f32,
    pub top_p: f32,
    pub repeat_penalty: f32
}

#[derive(Deserialize)]
pub struct OllamaGenerateRequest {
    pub model: String,
    pub prompt: String,
    pub stream: bool,
    pub options: OllamaOptions
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
fn handle_generate(req: Request) -> Result<Response> {
    println!("Handling request to {:?}", req.header("spin-full-url"));

    let request: OllamaGenerateRequest = serde_json::from_slice(req.body())?;

    println!("PROMPT: {:?}", &request.prompt);

    let result = infer_with_options(
        InferencingModel::Llama2Chat, 
        &request.prompt,
        InferencingParams {
            max_tokens: request.options.num_predict,
            repeat_penalty: request.options.repeat_penalty,
            repeat_penalty_last_n_token_count: 64,
            temperature: request.options.temperature,
            top_k: 40,
            top_p: request.options.top_p,
        }
    );
    
    let response = OllamaGenerateResponse { 
        model: String::from("llama2-chat:13b"), 
        response: result?.text, 
        done: true
    };

    send_ok_response(200, response)
}

fn send_ok_response(code: u16, resp: OllamaGenerateResponse) -> Result<Response> {
    Ok(spin_sdk::http::Response::builder()
        .status(code)
        .header("content-type", "application/json")
        .body(Some(serde_json::to_string(&resp)?))
        .build()
    )
}