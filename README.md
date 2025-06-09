# 🌀 Maze Generator for Unity

This is a simple maze generator for Unity using the DFS (Depth-First Search) algorithm. It builds a perfect maze by removing walls between grid cells and supports both 2D and 3D usage.

---

## 🧩 How It Works

- The grid is constructed using alternating cells: **wall – path – wall**
- DFS is used to generate a maze from the starting cell `(0, 0)`
- The maze is generated **step-by-step** using a `Coroutine`
- Every **odd coordinate** cell (like `(1,1)`, `(3,1)`, etc.) is a **Maze Cell**
- Walls between visited cells are removed to create passages

---

## ✅ How to Use

1. Create an **empty GameObject** in your Unity scene (e.g., `MazeGenerator`)
2. Attach the script `MazeCellGenerator.cs` to it
3. Create a **cube prefab** (recommended size: `0.25 x 0.25`) – this will act as a wall block
4. Assign the prefab to the `blockPrefab` field in the Inspector
5. Set your desired maze dimensions with `numX` and `numY`

You can use this in both 2D and 3D views depending on your camera setup and materials.

---

## 🧪 Demo and Advanced Version

There is a more polished version of this script available on GitHub.

---

## 🔗 Join the Community

For more free Unity content and tutorials in Ukrainian, check out the Telegram channel:  
👉 **[https://t.me/GameDevelopmentUA](https://t.me/GameDevelopmentUA)**

---

## 📜 License

This project is **free and open-source**.  
You can use, modify, and distribute it for personal, educational, or commercial purposes **without any restrictions**.

---

👾 Author: [Your name or nickname (optional)]

#Unity #MazeGenerator #CSharp #GameDev #OpenSource
