using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Social.Infrastructure.Twitter
{
    internal class UserByUsernameData
    {
        #region Default Fields

        /// <summary>
        /// The unique identifier of this user.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        /// <summary>
        /// The name of the user, as they’ve defined it on their profile. Not necessarily a person’s name. Typically capped at 50 characters, but subject to change.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Twitter screen name, handle, or alias that this user identifies themselves with.
        /// Usernames are unique but subject to change. Typically a maximum of 15 characters long,
        /// but some historical accounts may exist with longer names.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = default!;

        #endregion

        #region Optional Fields

        /// <summary>
        /// The UTC datetime that the user account was created on Twitter.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// The text of this user's profile description (also known as bio), if the user provided one.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The location specified in the user's profile, if the user provided one.
        /// As this is a freeform value, it may not indicate a valid location,
        /// but it may be fuzzily evaluated when performing searches with location queries.
        /// </summary>
        [JsonPropertyName("location")]
        public string? Location { get; set; }

        /// <summary>
        /// Unique identifier of this user's pinned Tweet.
        /// </summary>
        [JsonPropertyName("pinned_tweet_id")]
        public string? PinnedTweetId { get; set; }

        /// <summary>
        /// The URL to the profile image for this user, as shown on the user's profile.
        /// </summary>
        [JsonPropertyName("profile_image_url")]
        public Uri? ProfileImageUrl { get; set; }

        /// <summary>
        /// Indicates if this user has chosen to protect their Tweets (in other words, if this user's Tweets are private).
        /// </summary>
        [JsonPropertyName("protected")]
        public bool Protected { get; set; }

        /// <summary>
        /// Contains details about activity for this user.
        /// </summary>
        [JsonPropertyName("public_metrics")]
        public PublicMetrics? PublicMetrics { get; set; }

        /// <summary>
        /// The URL specified in the user's profile, if present.
        /// </summary>
        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        /// <summary>
        /// Indicates if this user is a verified Twitter User.
        /// </summary>
        [JsonPropertyName("verified")]
        public bool? Verified { get; set; }

        #endregion
    }

    internal class PublicMetrics
    {
        [JsonPropertyName("followers_count")]
        public int FollowersCount { get; set; } = default!;
        [JsonPropertyName("following_count")]
        public int FollowingCount { get; set; } = default!;
        [JsonPropertyName("tweet_count")]
        public int TweetCount { get; set; } = default!;
        [JsonPropertyName("listed_count")]
        public int ListedCount { get; set; } = default!;
    }
}
