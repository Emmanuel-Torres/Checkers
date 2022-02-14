import axios from "axios";
import User from "../models/user";

const authUrl = '/api/auth'

const authenticateUser = async (token: string): Promise<User> => {
    const res = await axios.post<User>(authUrl, token);
    return res.data;
}

const authService = {
    authenticateUser,
}

export default authService;