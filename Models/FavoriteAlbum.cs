using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ITunesSearchApp.Models
{
    [Table("favorite_albums")]
    public class FavoriteAlbum : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("itunes_collection_id")]
        public long ItunesCollectionId { get; set; }

        [Column("collection_name")]
        public string CollectionName { get; set; } = string.Empty;

        [Column("artist_name")]
        public string ArtistName { get; set; } = string.Empty;

        [Column("artwork_url_100")]
        public string? ArtworkUrl100 { get; set; }

        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [Column("primary_genre_name")]
        public string? PrimaryGenreName { get; set; }

        [Column("collection_view_url")]
        public string? CollectionViewUrl { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("is_featured")]
        public bool IsFeatured { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}