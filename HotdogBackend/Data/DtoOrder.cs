public record DtoOrder
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public List<DtoOrderItem> Items { get; set; }
    public float TotalPrice { get; set; }

}

public record DtoOrderItem
{
    public string Name { get; set; }
    public float Price { get; set; }
}