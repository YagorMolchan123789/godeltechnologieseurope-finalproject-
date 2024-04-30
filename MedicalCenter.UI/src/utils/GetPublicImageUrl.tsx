const getPublicImageUrlByName = (name: string) => {
    return require(`../public/images/${name}`);
};

export default getPublicImageUrlByName;
