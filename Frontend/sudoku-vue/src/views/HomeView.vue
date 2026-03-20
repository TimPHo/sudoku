<script setup lang="ts">
import { computed, ref } from 'vue'

type SudokuResponseDto = {
  board: number[][]
  solved: boolean
  message: string
}

const API_BASE_URL = 'https://localhost:7237'   // change to match backend port
const board = ref<number[][]>(createEmptyBoard())
const message = ref('')
const isLoading = ref(false)

const displayBoard = computed(() => board.value)

function createEmptyBoard(): number[][] {
  return Array.from({ length: 9 }, () => Array(9).fill(0))
}

function onCellInput(row: number, col: number, event: Event) {
  const input = event.target as HTMLInputElement
  const raw = input.value.trim()

  if (raw === '') {
    board.value[row][col] = 0
    input.value = ''
    return
  }

  const value = Number(raw)

  if (!Number.isInteger(value) || value < 1 || value > 9) {
    board.value[row][col] = 0
    input.value = ''
    return
  }

  board.value[row][col] = value
  input.value = String(value)
}

async function solveBoard() {
  isLoading.value = true
  message.value = ''

  try {
    const response = await fetch(`${API_BASE_URL}/api/Sudoku/solve`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        board: board.value,
      }),
    })

    if (!response.ok) {
      const errorText = await response.text()
      message.value = errorText || 'Unable to solve puzzle.'
      return
    }

    const data = (await response.json()) as SudokuResponseDto

    board.value = data.board
    message.value = data.message || (data.solved ? 'Solved.' : 'No solution found.')
  } catch {
    message.value = 'Cannot reach Sudoku API. Make sure backend is running and CORS is enabled.'
  } finally {
    isLoading.value = false
  }
}

function clearBoard() {
  board.value = createEmptyBoard()
  message.value = ''
}

function boxClass(row: number, col: number) {
  return {
    'box-right': col === 2 || col === 5,
    'box-bottom': row === 2 || row === 5,
  }
}
</script>

<template>
  <main class="page">
    <h1>Sudoku Solver</h1>
    <p class="subtitle">Enter numbers 1–9. Leave blank for empty cells.</p>

    <div class="board">
      <div v-for="(row, rowIndex) in displayBoard" :key="rowIndex" class="row">
        <input
          v-for="(cell, colIndex) in row"
          :key="`${rowIndex}-${colIndex}`"
          :value="cell === 0 ? '' : cell"
          type="text"
          inputmode="numeric"
          maxlength="1"
          class="cell"
          :class="boxClass(rowIndex, colIndex)"
          @input="onCellInput(rowIndex, colIndex, $event)"
        />
      </div>
    </div>

    <div class="actions">
      <button @click="solveBoard" :disabled="isLoading">
        {{ isLoading ? 'Solving...' : 'Solve' }}
      </button>

      <button type="button" class="secondary" @click="clearBoard" :disabled="isLoading">
        Clear
      </button>
    </div>

    <p v-if="message" class="message">{{ message }}</p>
  </main>
</template>

<style scoped>
.page {
  max-width: 520px;
  margin: 0 auto;
  padding: 24px;
  text-align: center;
}

h1 {
  margin-bottom: 8px;
}

.subtitle {
  margin-bottom: 20px;
  color: #555;
}

.board {
  display: inline-block;
  border: 3px solid #222;
  background: white;
}

.row {
  display: flex;
}

.cell {
  width: 44px;
  height: 44px;
  border: 1px solid #bbb;
  text-align: center;
  font-size: 20px;
  outline: none;
}

.cell:focus {
  background: #eef5ff;
}

.box-right {
  border-right: 3px solid #222;
}

.box-bottom {
  border-bottom: 3px solid #222;
}

.actions {
  margin-top: 20px;
  display: flex;
  justify-content: center;
  gap: 12px;
}

button {
  padding: 10px 18px;
  font-size: 16px;
  cursor: pointer;
}

.secondary {
  background: #f3f3f3;
}

.message {
  margin-top: 16px;
  font-weight: 600;
}
</style>