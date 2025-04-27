from fastapi import FastAPI
import torch
from shap_e.models.download import load_model
from shap_e.diffusion.sample import sample_latents
from shap_e.util.notebooks import decode_latent_mesh

app = FastAPI()
device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')

# Load models
model = load_model('text300M', device)
xm = load_model('transmitter', device)

@app.post("/generate")
async def generate_3d(prompt: str):
    latents = sample_latents(model, [prompt], device=device, batch_size=1)
    mesh = decode_latent_mesh(xm, latents[0])
    mesh.save("building.obj")
    return {"message": "3D model generated!", "file": "building.obj"}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)