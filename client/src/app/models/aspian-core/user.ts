export interface IUser {
  userName: string;
  email: string;
  displayName: string;
  bio: string;
  token: string;
  profilePhotoName: string;
}

export interface IUserFormValues {
  email: string;
  password: string;
  displayName?: string;
  username?: string;
}
