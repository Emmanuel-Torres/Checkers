import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import ProfileUpdateRequest from "../models/proflie-update-request";
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
  async (args: { token: string; update: ProfileUpdateRequest }, thunkApi: any): Promise<User> => {
    await authService.updateProfile(args.token, args.update);
    return await authService.authenticateUser(args.token);
  }
);

export const logout = createAsyncThunk(
  "logout",
  async (token: string, thunkApi: any) => {
    await authService.logout(token);
  }
);

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
  },
  extraReducers: (builder) => {
    builder
      .addCase(
        authenticateUser.fulfilled,
        (state, action: PayloadAction<User>) => {
          state.userProfile = action.payload;
        }
      )
      .addCase(authenticateUser.rejected, (state) => {
        state.userProfile = undefined;
      })
      .addCase(
        updateProfile.fulfilled,
        (state, action: PayloadAction<User>) => {
          state.userProfile = action.payload;
        }
      )
      .addCase(logout.fulfilled, (state) => {
        state.userProfile = undefined;
        state.userToken = undefined;
      });
  },
});

export const { setToken } = authSlice.actions;
export default authSlice;
