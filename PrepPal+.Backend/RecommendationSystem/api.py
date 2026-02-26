from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from models import RecommendationRequest
from vector_util import recommendationAlg

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=[
        "http://localhost:5173",   
        "http://localhost:3000",   
        "https://localhost:7101",  
    ],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.post("/recommend")
def recommend(request: RecommendationRequest):
    recs = recommendationAlg(request.likes, request.recipes)
    return recs