import axios from "axios";
import User from "../models/user";

const authUrl = "/api/auth";

const authenticateUser = async (token: string): Promise<User> => {
  const res = await axios.get<User>(authUrl + "/profile", {
    headers: { Authorization: `Bearer ${token}` },
  });
  return res.data;
};

const updateProfile = async (token: string, user: User) => {
  await axios.put(authUrl + "/profile", user, {
    headers: { Authorization: `Bearer ${token}` },
  });
};

const logout = async (token: string) => {
  await axios.post(authUrl + "/logout", token, {
    headers: { Authorization: `Bearer ${token}` },
  });
};

const authService = {
  authenticateUser,
  updateProfile,
  logout
};

export default authService;
