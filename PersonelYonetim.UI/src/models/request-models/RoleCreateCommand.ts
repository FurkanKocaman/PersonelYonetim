export interface RoleCreateCommand {
  roleName: string;
  yapisalRolMu: boolean;
  claims: string[];
  aciklama: string | undefined;
}
