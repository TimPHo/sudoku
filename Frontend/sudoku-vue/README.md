# Sudoku Vue App

A simple Sudoku web application built with **Vue 3 + Vite**.  
This app allows users to enter Sudoku puzzles, test solutions, and interact with a clean UI.

## Features

- 9x9 Sudoku board UI
- Input numbers (1–9) with validation
- Highlight original vs user-entered values
- Communicates with backend API for solving
- Simple and responsive interface

## Tech Stack

- Vue 3 (Composition API)
- Vite
- TypeScript
- Axios (API calls)

## Project Structure
src/
├── components/ # Reusable UI components (e.g., SudokuBoard)
├── views/ # Main pages (e.g., HomeView)
├── services/ # API calls
├── ultilities/ # Helper methods
├── models/ # Data models / types
├── App.vue
└── main.ts

## Getting Started

### Install dependencies 
Run locally
```bash
npm install
npm run dev
```
App will run at:
http://localhost:5173

## Build for production
npm run build

## Backend API

This frontend connects to the Sudoku API:

Default: https://localhost/7237

Make sure backend is running before using solve feature

## Demo
https://proud-moss-0a980341e.4.azurestaticapps.net/

## Notes
This project was built as a learning/demo application
Only minimal features are implemented; improvements are welcome

## Future Improvements
Auto-solve visualization
Difficulty level
Better UI/UX styling
Error highlighting
Mobile optimization
Tests & UI validation