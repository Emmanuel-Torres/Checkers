import axios from "axios";
import ProfileUpdateRequest from "../models/proflie-update-request";
import User from "../models/user";

const authUrl = "/api/auth";

const authenticateUser = async (token: string): Promise<User> => {
  const res = await axios.get<User>(authUrl + "/profile", {
    headers: { Authorization: `Bearer ${token}` },
  });
  return res.data;
};

const updateProfile = async (token: string, update: ProfileUpdateRequest) => {
  const formData = new FormData();
  formData.append("bestJoke", update.bestJoke ?? "");
  formData.append("iceCreamFlavor", update.iceCreamFlavor ?? "");
  formData.append("pizza", update.pizza ?? "");
  formData.append("picture", update.picture ?? "");
  formData.append("age", update.age?.toString() ?? "0");
  await axios.put(authUrl + "/profile", formData, {
    headers: {
      Authorization: `Bearer ${token}`
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
