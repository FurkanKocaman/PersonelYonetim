export interface PasswordResetModel {
  userId: string;
  token: string;
  newPassword: string;
}
