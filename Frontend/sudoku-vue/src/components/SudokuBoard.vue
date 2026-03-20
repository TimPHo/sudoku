<script setup lang="ts">
type Props = {
  board: number[][]
  originalBoard: number[][]
}

const props = defineProps<Props>()

const emit = defineEmits<{
  cellInput: [row: number, col: number, event: Event]
  cellKeydown: [row: number, col: number, event: KeyboardEvent]
}>()

function getCellId(row: number, col: number): string {
  return `cell-${row}-${col}`
}

function boxClass(row: number, col: number) {
  return {
    'box-right': col === 2 || col === 5,
    'box-bottom': row === 2 || row === 5,
  }
}

function isSolvedCell(row: number, col: number): boolean {
  return props.originalBoard[row]![col] === 0 && props.board[row]![col] !== 0
}
</script>

<template>
  <div class="board">
    <div v-for="(row, rowIndex) in board" :key="rowIndex" class="row">
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
        @input="emit('cellInput', rowIndex, colIndex, $event)"
        @keydown="emit('cellKeydown', rowIndex, colIndex, $event)"
      />
    </div>
  </div>
</template>