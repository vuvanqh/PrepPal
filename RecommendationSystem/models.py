import pydantic as py
from typing import *
from pydantic import BaseModel
from uuid import UUID

class Recipe(BaseModel):
    recipeId: UUID
    category: str
    area: str
    ingredients: list[str]

class RecommendationRequest(BaseModel):
    likes: list[UUID]
    recipes: list[Recipe]

