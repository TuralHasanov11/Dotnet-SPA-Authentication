import type { AppError } from './Error'

export class Result<T> {
  public readonly value?: T
  public readonly isSuccess: boolean
  public readonly error?: AppError

  private constructor(value?: T, error?: AppError) {
    if (error) {
      this.error = error
      this.isSuccess = false
    } else {
      this.value = value
      this.isSuccess = true
    }
  }

  public static success<T>(value: T): Result<T> {
    return new Result<T>(value)
  }

  public static failure<T>(error: AppError): Result<T> {
    return new Result<T>(undefined, error)
  }

  public static create<T>(value: T): Result<T> {
    return new Result<T>(value)
  }

  public get isFailure(): boolean {
    return !this.isSuccess
  }
}
