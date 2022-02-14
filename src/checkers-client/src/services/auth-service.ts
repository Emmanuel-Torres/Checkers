import axios from "axios";
import User from "../models/user";

const authUrl = '/api/auth'

const authenticate = async (token: string): Promise<User> => {
    const res = await axios.post(authUrl, token);
    return res.data;
}

const authService = {
    authenticate,
}

export default authService;