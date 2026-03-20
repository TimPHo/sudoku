<script setup lang="ts">
import { computed, ref } from 'vue'
import { API_BASE_URL } from '@/config'

type SudokuResponseDto = {
  board: number[][]
  solved: boolean
  message: string
}

const board = ref<number[][]>(createEmptyBoard())
const originalBoard = ref<number[][]>(createEmptyBoard())
const message = ref('')
const isLoading = ref(false)

function createEmptyBoard(): number[][] {
  return Array.from({ length: 9 }, () => Array(9).fill(0))
}

function createTestBoard(): number[][] {
  return [
    [5, 3, 0, 0, 7, 0, 0, 0, 0],
    [6, 0, 0, 1, 9, 5, 0, 0, 0],
    [0, 9, 8, 0, 0, 0, 0, 6, 0],
    [8, 0, 0, 0, 6, 0, 0, 0, 3],
    [4, 0, 0, 8, 0, 3, 0, 0, 1],
    [7, 0, 0, 0, 2, 0, 0, 0, 6],
    [0, 6, 0, 0, 0, 0, 2, 8, 0],
    [0, 0, 0, 4, 1, 9, 0, 0, 5],
    [0, 0, 0, 0, 8, 0, 0, 7, 9],
  ]
}

function cloneBoard(source: number[][]): number[][] {
  return source.map(row => [...row])
}

const displayBoard = computed(() => board.value)

function getCellId(row: number, col: number): string {
  return `cell-${row}-${col}`
}

function focusCell(row: number, col: number) {
  if (row < 0 || row > 8 || col < 0 || col > 8) return

  const element = document.getElementById(getCellId(row, col)) as HTMLInputElement | null
  element?.focus()
  element?.select()
}

function moveToNextCell(row: number, col: number) {
  if (col < 8) {
    focusCell(row, col + 1)
    return
  }

  if (row < 8) {
    focusCell(row + 1, 0)
  }
}

function onCellInput(row: number, col: number, event: Event) {
  const input = event.target as HTMLInputElement
  const raw = input.value.trim()

  if (raw === '') {
    board.value[row]![col] = 0
    input.value = ''
    return
  }

  const lastChar = raw.slice(-1)
  const value = Number(lastChar)

  if (!Number.isInteger(value) || value < 1 || value > 9) {
    board.value[row]![col] = 0
    input.value = ''
    return
  }

  board.value[row]![col] = value
  input.value = String(value)

  moveToNextCell(row, col)
}

function onCellKeydown(row: number, col: number, event: KeyboardEvent) {
  switch (event.key) {
    case 'ArrowUp':
      event.preventDefault()
      focusCell(row - 1, col)
      break
    case 'ArrowDown':
      event.preventDefault()
      focusCell(row + 1, col)
      break
    case 'ArrowLeft':
      event.preventDefault()
      focusCell(row, col - 1)
      break
    case 'ArrowRight':
      event.preventDefault()
      focusCell(row, col + 1)
      break
    case 'Backspace':
    case 'Delete':
      board.value[row]![col] = 0
      break
  }
}

async function solveBoard() {
  isLoading.value = true
  message.value = ''

  try {
    originalBoard.value = cloneBoard(board.value)

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
  originalBoard.value = createEmptyBoard()
  message.value = ''
  focusCell(0, 0)
}

function loadTestBoard() {
  board.value = createTestBoard()
  originalBoard.value = createEmptyBoard()
  message.value = 'Test puzzle loaded.'
  focusCell(0, 0)
}

function boxClass(row: number, col: number) {
  return {
    'box-right': col === 2 || col === 5,
    'box-bottom': row === 2 || row === 5,
  }
}

function isSolvedCell(row: number, col: number): boolean {
  return originalBoard.value[row]![col] === 0 && board.value[row]![col] !== 0
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
          :id="getCellId(rowIndex, colIndex)"
          :key="`${rowIndex}-${colIndex}`"
          :value="cell === 0 ? '' : cell"
          type="text"
          inputmode="numeric"
          maxlength="1"
          class="cell"
          :class="[boxClass(rowIndex, colIndex), { 'solved-cell': isSolvedCell(rowIndex, colIndex) }]"
          @input="onCellInput(rowIndex, colIndex, $event)"
          @keydown="onCellKeydown(rowIndex, colIndex, $event)"
        />
      </div>
    </div>

    <div class="actions">
      <button type="button" class="secondary" @click="loadTestBoard" :disabled="isLoading">
        Test
      </button>

      <button type="button" class="secondary" @click="clearBoard" :disabled="isLoading">
        Clear
      </button>

      <button type="button" @click="solveBoard" :disabled="isLoading">
        {{ isLoading ? 'Solving...' : 'Solve' }}
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
  font-weight: 600;
  color: #222;
  outline: none;
}

.cell:focus {
  background: #eef5ff;
}

.solved-cell {
  color: #2563eb;
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