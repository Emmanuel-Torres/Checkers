import axios from "axios";
import User from "../models/user";

const authUrl = '/api/auth'

const authenticateUser = async (token: string): Promise<User> => {
    const res = await axios.get<User>(authUrl + '/profile', { headers: { "Authorization": `Bearer ${token}`}});
    console.log("Here", res.data);
    return res.data;
}

const authService = {
    authenticateUser,
}

export default authService;