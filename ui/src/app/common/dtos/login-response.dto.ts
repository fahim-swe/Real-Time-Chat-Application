export interface LoginResponseDto {
  data: {
    id: string;
    userId: string;
    token: string;
    userName: string;
    refreshToken: string;
    expiredTime: Date;
  };
  succeeded: boolean;
  errors: any;
  message: string;
}
