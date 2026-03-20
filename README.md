# Sudoku Solver

Full-stack Sudoku solver with a Vue frontend and ASP.NET Core backend.

## Tech Stack
- Frontend: Vue 3 + Vite
- Backend: ASP.NET Core Web API

## Project Structure
- `Frontend/sudoku-vue` – Vue app
- `Backend/SudokuApi` – API

## Run Backend
cd Backend/SudokuApi  
dotnet run  

API runs on: https://localhost:7237

## Run Frontend
cd Frontend/sudoku-vue  
npm install  
npm run dev  

App runs on: http://localhost:5173

## Notes
- Make sure backend is running before using Solve
- Check CORS if API calls fail