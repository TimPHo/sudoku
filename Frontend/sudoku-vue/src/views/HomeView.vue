<script setup lang="ts">
import { ref } from 'vue'
import { API_BASE_URL } from '@/config'
import SudokuBoard from '@/components/SudokuBoard.vue'
import { cloneBoard, createEmptyBoard, createTestBoard } from '@/utilities/sudoku'

type SudokuResponseDto = {
  board: number[][]
  solved: boolean
  message: string
}

const board = ref<number[][]>(createEmptyBoard())
const originalBoard = ref<number[][]>(createEmptyBoard())
const message = ref('')
const isLoading = ref(false)

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
</script>

<template>
  <main class="page">
    <h1>Sudoku Solver</h1>
    <p class="subtitle">Enter numbers 1–9. Leave blank for empty cells.</p>

    <SudokuBoard
      :board="board"
      :original-board="originalBoard"
      @cell-input="onCellInput"
      @cell-keydown="onCellKeydown"
    />

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
  padding: 8px 24px 24px;
  text-align: center;
}

h1 {
  margin-top: 0;
  margin-bottom: 8px;
}

.subtitle {
  margin-bottom: 20px;
  color: #facc15;
}

:deep(.board) {
  display: inline-block;
  border: 3px solid #222;
  background: white;
}

:deep(.row) {
  display: flex;
}

:deep(.cell) {
  width: 33px;
  height: 33px;
  border: 1px solid #bbb;
  text-align: center;
  font-size: 20px;
  font-weight: 600;
  color: #222;
  outline: none;
}

:deep(.cell:focus) {
  background: #eef5ff;
}

:deep(.solved-cell) {
  color: #2563eb;
}

:deep(.box-right) {
  border-right: 3px solid #222;
}

:deep(.box-bottom) {
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