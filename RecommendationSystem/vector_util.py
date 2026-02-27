import numpy as np
# class Likes(BaseModel):
#     userId: UUID
#     likes: list[str]

# class Recipe(BaseModel):
#     recipeId: UUID
#     category: str
#     area: str
#     ingredients: list[str]

# class RecommendationRequest(BaseModel):
#     likes: list[Likes]
#     recipes: list[Recipe]



def vocab(values):
    return {c: i for i, c in enumerate(sorted(set(values)))}


#each recipe represented as [...onehot_category, ...onehot_area, ...encoded_ingredients]

def encode(index, size):
    vec = np.zeros(size)
    vec[index] = 1
    return vec

def encode_recipe(r, c_vocab, a_vocab, i_vocab):
    cat = encode(c_vocab[r.category], len(c_vocab))
    area = encode(a_vocab[r.area], len(a_vocab))

    ingredients = np.zeros(len(i_vocab))
    for i in r.ingredients:
        ingredients[i_vocab[i]] = 1

    return np.concatenate([cat*1.5,area*2,ingredients*1])

def encode_user(liked_recipe_ids, recipe_encodings):
    vectors = [
        recipe_encodings[rid]
        for rid in liked_recipe_ids
        if rid in recipe_encodings
    ]

    if not vectors:
        return np.zeros(next(iter(recipe_encodings.values())).shape)
    
    return np.mean(vectors, axis=0)

def cos_similarity(a, b):
    denom = np.linalg.norm(a) * np.linalg.norm(b)
    if denom == 0:
        return 0.0
    return np.dot(a, b) / denom

def recommend(liked_recipe_ids, user_encoding, recipe_encoding):
    like_matrix = []

    for recipeId, vec in recipe_encoding.items():
        if recipeId in liked_recipe_ids:
            continue
        like_matrix.append((recipeId, cos_similarity(vec, user_encoding)))

    return sorted(like_matrix, key=lambda x: x[1], reverse=True)


def recommendationAlg(liked_recipe_ids, recipes):
    categories = []; areas =[]; ingredients = []

    for r in recipes:
        categories.append(r.category)
        areas.append(r.area)
        ingredients.extend(r.ingredients)

    category_vocab = vocab(categories)
    area_vocab = vocab(areas)
    ingredients_vocab = vocab(ingredients)

    recipe_encodings = {r.recipeId:encode_recipe(r,category_vocab, area_vocab, ingredients_vocab)
                         for r in recipes}
    
    recommendations = []
    user_encoding = encode_user(liked_recipe_ids, recipe_encodings)

    ranked = recommend(liked_recipe_ids, user_encoding, recipe_encodings)
    return [rid for rid, score in ranked]

