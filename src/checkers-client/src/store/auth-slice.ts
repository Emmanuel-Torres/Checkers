import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import User from "../models/user";
import authService from "../services/auth-service";

export const authenticateUser = createAsyncThunk(
  "authenticateUser",
  async (token: string, thunkApi: any): Promise<User> => {
    return await authService.authenticateUser(token);
  }
);

export const updateProfile = createAsyncThunk(
  "updateProfile",
  async (args: {token: string, user: User}, thunkApi: any): Promise<User> => {
    await authService.updateProfile(args.token, args.user);
    return await authService.authenticateUser(args.token);
  }
)

interface AuthState {
  userToken?: string;
  userProfile?: User;
}

const initialState: AuthState = {
  userToken: undefined,
  userProfile: undefined,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setToken(state, action: PayloadAction<string>) {
      state.userToken = action.payload;
    },
    logout(state) {
      state.userToken = undefined;
      state.userProfile = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(
      authenticateUser.fulfilled, (state, action: PayloadAction<User>) => {
        state.userProfile = action.payload;
      }
    ).addCase(authenticateUser.rejected, (state, action) => {
      state.userProfile = undefined;
    });
  },
});

export const { setToken, logout } = authSlice.actions;
export default authSlice;
