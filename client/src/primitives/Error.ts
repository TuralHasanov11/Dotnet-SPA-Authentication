import type { AxiosError } from 'axios'

export type ResponseError = AxiosError

export enum ErrorType {
  Failure = 0,
  Validation = 1,
  NotFound = 2,
  Conflict = 3
}

export class AppError {
  public readonly message: string
  public readonly errorType: ErrorType

  private constructor(message: string, errorType: ErrorType) {
    this.message = message
    this.errorType = errorType
  }

  public static notFound(message: string): AppError {
    return new AppError(message, ErrorType.NotFound)
  }

  public static conflict(message: string): AppError {
    return new AppError(message, ErrorType.Conflict)
  }

  public static validation(message: string): AppError {
    return new AppError(message, ErrorType.Validation)
  }

  public static failure(message: string): AppError {
    return new AppError(message, ErrorType.Failure)
  }
}
