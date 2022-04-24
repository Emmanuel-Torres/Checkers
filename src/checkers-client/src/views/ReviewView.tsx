import { ChangeEvent, FC, FormEvent, useEffect, useState } from "react";
import Reviews from "../components/reviews/Reviews";
import Review from "../models/review";
import reviewService from "../services/review-service";
import { useStoreSelector } from "../store";

const ReviewView: FC = (): JSX.Element => {
    const token = useStoreSelector(state => state.auth.userToken);

    const [reviews, setReviews] = useState<Review[]>([]);
    const [review, setReview] = useState("");

    useEffect(() => {
        reviewService.getReviews().then(r => setReviews(r));
    }, []);

    const reviewChangedHandler = (e: ChangeEvent<HTMLTextAreaElement>) => {
        setReview(e.target.value);
    }

    const submitComment = async (e: FormEvent) => {
        e.preventDefault();
        if (review.trim().length > 0 && review.trim().length <= 1000) {
            if (token) {
                await reviewService.addReviewAsUser(review, token);
            } else {
                await reviewService.addReviewAsAnonymous(review);
            }

            const res = await reviewService.getReviews();
            setReviews(res);
        }
    }

    return <>
        <h2>This is the review page</h2>
        <p>Leave a review</p>
        <form onSubmit={submitComment}>
            <label htmlFor="review">Write your review</label><br/>
            <textarea name="review" value={review} onChange={reviewChangedHandler} />
            <p>{review.trim().length}/1000 characters used</p>
            {review.trim().length >= 1000 && <p>Review too large</p>}
            <button type="submit">Add Review</button>
        </form>
        <Reviews reviews={reviews} />
    </>

}

export default ReviewView;
