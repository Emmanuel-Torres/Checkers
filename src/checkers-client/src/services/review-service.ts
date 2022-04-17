import axios from "axios";
import Review from "../models/review";

const reviewUrl = "/api/review";

const addReviewAsUser = async (content: string, token: string) => {
  await axios.post(reviewUrl, content, {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  });
};

const addReviewAsAnonymous = async (content: string) => {
  await axios.post(reviewUrl, content, {
    headers: { "Content-Type": "application/json" },
  });
};

const getReviews = async (): Promise<Review[]> => {
  var res = await axios.get<Review[]>(reviewUrl);
  return res.data;
};

const reviewService = {
  addReviewAsUser,
  addReviewAsAnonymous,
  getReviews,
};

export default reviewService;
