export function createEmptyBoard(): number[][] {
  return Array.from({ length: 9 }, () => Array(9).fill(0))
}

export function createTestBoard(): number[][] {
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

export function cloneBoard(source: number[][]): number[][] {
  return source.map(row => [...row])
}

export function isValidCellValue(value: string): number {
  const lastChar = value.trim().slice(-1)
  const num = Number(lastChar)
  return Number.isInteger(num) && num >= 1 && num <= 9 ? num : 0
}