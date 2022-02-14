import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import User from "../models/user";
import authService from "../services/auth-service";

export const authenticateUser = createAsyncThunk(
    'authenticateUser',
    async (token: string, thunkApi): Promise<User> => {
        return await authService.authenticate(token);
    }
)

interface AuthState {
    userToken?: string,
    userProfile?: User
}

const initialState: AuthState = {
    userToken: undefined,
    userProfile: undefined
}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {

    },
    extraReducers: (builder) => {
        builder
            .addCase(authenticateUser.fulfilled, (state, action: PayloadAction<User>) => {
                state.userProfile = action.payload;
            })
    }
})

export default authSlice;