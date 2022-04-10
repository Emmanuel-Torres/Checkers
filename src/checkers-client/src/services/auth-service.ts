import axios from "axios";
import UpdateProfileRequest from "../models/update-proflie-request";
import User from "../models/user";

const authUrl = "/api/auth";

const authenticateUser = async (token: string): Promise<User> => {
  const res = await axios.get<User>(authUrl + "/profile", {
    headers: { Authorization: `Bearer ${token}` },
  });
  return res.data;
};

const updateProfile = async (token: string, update: UpdateProfileRequest) => {
  console.log(update);
  await axios.put(authUrl + "/profile", update, {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "multipart/form-data",
    },
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
  logout,
};

export default authService;
