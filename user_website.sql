-- phpMyAdmin SQL Dump
-- version 4.9.5deb2
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost:3306
-- Généré le : lun. 07 mars 2022 à 21:11
-- Version du serveur :  10.6.7-MariaDB-1:10.6.7+maria~focal
-- Version de PHP : 7.4.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `user_website`
--

-- --------------------------------------------------------

--
-- Structure de la table `rp_api`
--

CREATE TABLE `rp_api` (
  `playerid` int(20) NOT NULL,
  `name` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `steamid_32` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `steamid_64` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `tag` varchar(64) COLLATE utf8mb3_bin NOT NULL DEFAULT 'N/A',
  `country` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `ip` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `admin` int(1) NOT NULL DEFAULT 0,
  `tutorial` int(1) NOT NULL,
  `nationality` int(10) NOT NULL,
  `sexe` int(10) NOT NULL,
  `jobid` int(10) NOT NULL,
  `gradeid` int(10) NOT NULL,
  `level` int(10) NOT NULL,
  `xp` int(10) NOT NULL,
  `money` int(10) NOT NULL,
  `bank` int(10) NOT NULL,
  `playtime` int(10) NOT NULL,
  `viptime` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `rp_api`
--

INSERT INTO `rp_api` (`playerid`, `name`, `steamid_32`, `steamid_64`, `tag`, `country`, `ip`, `admin`, `tutorial`, `nationality`, `sexe`, `jobid`, `gradeid`, `level`, `xp`, `money`, `bank`, `playtime`, `viptime`) VALUES
(1, 'Бeиitѳ', 'STEAM_1:1:512215951', '76561198984697631', '{yellow}CODEUR', 'Belgium', '91.179.52.66', 1, 0, 0, 0, 0, 1, 0, 0, 0, 300, 0, 0),
(3, '[๖ۣۜSKß] Adrián', 'STEAM_1:0:15774088', '76561197991813904', '', 'Spain', '81.202.236.251', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- --------------------------------------------------------

--
-- Structure de la table `web_bans`
--

CREATE TABLE `web_bans` (
  `id` int(20) NOT NULL,
  `user` int(20) NOT NULL,
  `admin` int(10) NOT NULL,
  `time` int(32) NOT NULL,
  `raison` varchar(256) COLLATE utf8mb3_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

-- --------------------------------------------------------

--
-- Structure de la table `web_cart`
--

CREATE TABLE `web_cart` (
  `id` int(20) NOT NULL,
  `user` int(10) NOT NULL,
  `product` int(10) NOT NULL,
  `quantity` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_cart`
--

INSERT INTO `web_cart` (`id`, `user`, `product`, `quantity`) VALUES
(1, 2, 2, 25),
(2, 2, 2, 25);

-- --------------------------------------------------------

--
-- Structure de la table `web_confirmations`
--

CREATE TABLE `web_confirmations` (
  `id` int(20) NOT NULL,
  `code` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `confirmed` int(1) NOT NULL,
  `user` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_confirmations`
--

INSERT INTO `web_confirmations` (`id`, `code`, `confirmed`, `user`) VALUES
(2, 'EFY0WFKEPH', 1, 2);

-- --------------------------------------------------------

--
-- Structure de la table `web_forum_category`
--

CREATE TABLE `web_forum_category` (
  `cat_id` int(10) NOT NULL,
  `cat_node` int(10) NOT NULL,
  `cat_name` varchar(255) NOT NULL,
  `cat_description` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Déchargement des données de la table `web_forum_category`
--

INSERT INTO `web_forum_category` (`cat_id`, `cat_node`, `cat_name`, `cat_description`) VALUES
(7, 5, 'Annonces', 'Toutes les annonces communautaire sont ici 😁'),
(9, 5, 'Présentations', 'Brèves présentation des joueurs.'),
(10, 4, 'Liste à faire', 'patchnotes & to-do à faires.'),
(11, 6, 'Roleplay', 'Toutes les informations conçernant le serveur roleplay.'),
(12, 6, 'Jailbreak', 'Toutes les informations conçernant le serveur jailbreak.'),
(13, 6, 'Retakes', 'Toutes les informations conçernant le serveur retakes.'),
(14, 6, 'Only-Deagle', 'Toutes les informations conçernant le serveur only-deagle.'),
(16, 7, 'Roleplay', 'Toutes les informations conçernant le serveur roleplay.'),
(17, 5, 'Règlements', 'Règlements des serveurs & de la communauté');

-- --------------------------------------------------------

--
-- Structure de la table `web_forum_nodes`
--

CREATE TABLE `web_forum_nodes` (
  `node_id` int(10) NOT NULL,
  `node_name` varchar(255) NOT NULL,
  `node_role` int(1) NOT NULL DEFAULT 0,
  `node_image` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Déchargement des données de la table `web_forum_nodes`
--

INSERT INTO `web_forum_nodes` (`node_id`, `node_name`, `node_role`, `node_image`) VALUES
(4, 'Communauté', 1, 'las la-users'),
(5, 'Général', 0, 'las la-home'),
(6, 'Counter-Strike: Global Offensive', 0, 'assets/images/games/csgo.png'),
(7, 'S&Box', 0, 'assets/images/games/sbox.png');

-- --------------------------------------------------------

--
-- Structure de la table `web_forum_posts`
--

CREATE TABLE `web_forum_posts` (
  `post_id` int(8) NOT NULL,
  `post_content` text COLLATE utf8mb3_bin NOT NULL,
  `post_date` datetime NOT NULL DEFAULT current_timestamp(),
  `post_topic` int(8) NOT NULL,
  `post_by` int(8) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_forum_posts`
--

INSERT INTO `web_forum_posts` (`post_id`, `post_content`, `post_date`, `post_topic`, `post_by`) VALUES
(41, 'Fdoesesess', '2022-03-07 19:49:18', 2, 2);

-- --------------------------------------------------------

--
-- Structure de la table `web_forum_topics`
--

CREATE TABLE `web_forum_topics` (
  `topic_id` int(8) NOT NULL,
  `topic_subject` varchar(255) COLLATE utf8mb3_bin NOT NULL,
  `topic_content` text COLLATE utf8mb3_bin NOT NULL,
  `topic_date` datetime NOT NULL DEFAULT current_timestamp(),
  `topic_cat` int(8) NOT NULL,
  `topic_by` int(8) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_forum_topics`
--

INSERT INTO `web_forum_topics` (`topic_id`, `topic_subject`, `topic_content`, `topic_date`, `topic_cat`, `topic_by`) VALUES
(2, 'kING', 'Pourquoi est-ce que king est con ? C\'est un très bon point à remarquer sur lui car il est tellement con que l\'on le considerait comme un handicapé', '2022-03-03 04:03:42', 10, 2),
(3, 'Okay', '', '2022-03-03 04:03:42', 10, 2),
(4, 'dONOVAN', '', '2022-03-03 04:03:42', 10, 2),
(5, 'LES DEUX BG', '', '2022-03-03 04:03:42', 10, 2);

-- --------------------------------------------------------

--
-- Structure de la table `web_news`
--

CREATE TABLE `web_news` (
  `id` int(20) NOT NULL,
  `title` varchar(128) COLLATE utf8mb3_bin NOT NULL,
  `content` varchar(2048) COLLATE utf8mb3_bin NOT NULL,
  `date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_news`
--

INSERT INTO `web_news` (`id`, `title`, `content`, `date`) VALUES
(1, 'Accueil', 't', '2022-02-27 20:33:04');

-- --------------------------------------------------------

--
-- Structure de la table `web_patchnotes`
--

CREATE TABLE `web_patchnotes` (
  `id` int(20) NOT NULL,
  `title` varchar(128) COLLATE utf8mb3_bin NOT NULL,
  `content` varchar(2048) COLLATE utf8mb3_bin NOT NULL,
  `owner` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `status` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_patchnotes`
--

INSERT INTO `web_patchnotes` (`id`, `title`, `content`, `owner`, `status`, `date`) VALUES
(2, 'Test', 'donova', 'Benito', 'En cours', '2022-03-06 19:19:06');

-- --------------------------------------------------------

--
-- Structure de la table `web_products`
--

CREATE TABLE `web_products` (
  `id` int(20) NOT NULL,
  `name` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `price` int(20) NOT NULL,
  `imagefile` varchar(256) COLLATE utf8mb3_bin NOT NULL,
  `category` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `description` varchar(512) COLLATE utf8mb3_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_products`
--

INSERT INTO `web_products` (`id`, `name`, `price`, `imagefile`, `category`, `description`) VALUES
(1, 'Tokens', 1, '241747', 'home', 'EN DEVELOPMENT'),
(2, 'VIP', 10, 'vip.png', 'home', 'EN DEVELOPMENT');

-- --------------------------------------------------------

--
-- Structure de la table `web_servers`
--

CREATE TABLE `web_servers` (
  `id` int(20) NOT NULL,
  `ip` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `port` varchar(16) COLLATE utf8mb3_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

-- --------------------------------------------------------

--
-- Structure de la table `web_stats`
--

CREATE TABLE `web_stats` (
  `totalvisits` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_stats`
--

INSERT INTO `web_stats` (`totalvisits`) VALUES
(1406),
(1434);

-- --------------------------------------------------------

--
-- Structure de la table `web_svlicences`
--

CREATE TABLE `web_svlicences` (
  `id` int(20) NOT NULL,
  `ip` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `token` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `enabled` int(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_svlicences`
--

INSERT INTO `web_svlicences` (`id`, `ip`, `token`, `enabled`) VALUES
(1, '141.94.248.207:27315', 'LICENCE_54E7S6E94D', 1);

-- --------------------------------------------------------

--
-- Structure de la table `web_users`
--

CREATE TABLE `web_users` (
  `id` int(20) NOT NULL,
  `email` varchar(32) COLLATE utf8mb3_bin NOT NULL,
  `steamid` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `username` varchar(64) COLLATE utf8mb3_bin NOT NULL,
  `password` varchar(128) COLLATE utf8mb3_bin NOT NULL,
  `role` int(1) NOT NULL DEFAULT 0,
  `mail_confirmed` int(1) NOT NULL DEFAULT 0,
  `joindate` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;

--
-- Déchargement des données de la table `web_users`
--

INSERT INTO `web_users` (`id`, `email`, `steamid`, `username`, `password`, `role`, `mail_confirmed`, `joindate`) VALUES
(2, 'benitalpa1020@gmail.com', '76561198984697631', 'Benito', 'cde3fa6490822f47f485b508bb79ff2e09760a35c285a6c30240ddecbe8185e2', 1, 1, '2022-02-28 13:56:42');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `rp_api`
--
ALTER TABLE `rp_api`
  ADD PRIMARY KEY (`playerid`),
  ADD UNIQUE KEY `playerid` (`playerid`);

--
-- Index pour la table `web_bans`
--
ALTER TABLE `web_bans`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user` (`user`);

--
-- Index pour la table `web_cart`
--
ALTER TABLE `web_cart`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user` (`user`),
  ADD KEY `product` (`product`);

--
-- Index pour la table `web_confirmations`
--
ALTER TABLE `web_confirmations`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user` (`user`);

--
-- Index pour la table `web_forum_category`
--
ALTER TABLE `web_forum_category`
  ADD PRIMARY KEY (`cat_id`),
  ADD KEY `web_forum_category` (`cat_node`);

--
-- Index pour la table `web_forum_nodes`
--
ALTER TABLE `web_forum_nodes`
  ADD PRIMARY KEY (`node_id`);

--
-- Index pour la table `web_forum_posts`
--
ALTER TABLE `web_forum_posts`
  ADD PRIMARY KEY (`post_id`),
  ADD KEY `post_topic` (`post_topic`),
  ADD KEY `post_by` (`post_by`);

--
-- Index pour la table `web_forum_topics`
--
ALTER TABLE `web_forum_topics`
  ADD PRIMARY KEY (`topic_id`),
  ADD KEY `topic_cat` (`topic_cat`),
  ADD KEY `web_forum_topics` (`topic_by`);

--
-- Index pour la table `web_news`
--
ALTER TABLE `web_news`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `web_patchnotes`
--
ALTER TABLE `web_patchnotes`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `web_products`
--
ALTER TABLE `web_products`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `web_servers`
--
ALTER TABLE `web_servers`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `web_stats`
--
ALTER TABLE `web_stats`
  ADD PRIMARY KEY (`totalvisits`),
  ADD UNIQUE KEY `totalvisits` (`totalvisits`);

--
-- Index pour la table `web_svlicences`
--
ALTER TABLE `web_svlicences`
  ADD PRIMARY KEY (`id`);

--
-- Index pour la table `web_users`
--
ALTER TABLE `web_users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `web_bans`
--
ALTER TABLE `web_bans`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `web_cart`
--
ALTER TABLE `web_cart`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `web_confirmations`
--
ALTER TABLE `web_confirmations`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `web_forum_category`
--
ALTER TABLE `web_forum_category`
  MODIFY `cat_id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT pour la table `web_forum_nodes`
--
ALTER TABLE `web_forum_nodes`
  MODIFY `node_id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pour la table `web_forum_posts`
--
ALTER TABLE `web_forum_posts`
  MODIFY `post_id` int(8) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT pour la table `web_forum_topics`
--
ALTER TABLE `web_forum_topics`
  MODIFY `topic_id` int(8) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT pour la table `web_news`
--
ALTER TABLE `web_news`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `web_patchnotes`
--
ALTER TABLE `web_patchnotes`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `web_products`
--
ALTER TABLE `web_products`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `web_servers`
--
ALTER TABLE `web_servers`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `web_svlicences`
--
ALTER TABLE `web_svlicences`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT pour la table `web_users`
--
ALTER TABLE `web_users`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `web_bans`
--
ALTER TABLE `web_bans`
  ADD CONSTRAINT `web_bans_ibfk_1` FOREIGN KEY (`user`) REFERENCES `web_users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `web_cart`
--
ALTER TABLE `web_cart`
  ADD CONSTRAINT `web_cart_ibfk_1` FOREIGN KEY (`user`) REFERENCES `web_users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `web_cart_ibfk_2` FOREIGN KEY (`product`) REFERENCES `web_products` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `web_confirmations`
--
ALTER TABLE `web_confirmations`
  ADD CONSTRAINT `web_confirmations_ibfk_1` FOREIGN KEY (`user`) REFERENCES `web_users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `web_forum_category`
--
ALTER TABLE `web_forum_category`
  ADD CONSTRAINT `web_forum_category` FOREIGN KEY (`cat_node`) REFERENCES `web_forum_nodes` (`node_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `web_forum_category_ibfk_1` FOREIGN KEY (`cat_node`) REFERENCES `web_forum_nodes` (`node_id`) ON UPDATE CASCADE;

--
-- Contraintes pour la table `web_forum_posts`
--
ALTER TABLE `web_forum_posts`
  ADD CONSTRAINT `web_forum_posts_ibfk_1` FOREIGN KEY (`post_topic`) REFERENCES `web_forum_topics` (`topic_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `web_forum_posts_ibfk_2` FOREIGN KEY (`post_by`) REFERENCES `web_users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `web_forum_topics`
--
ALTER TABLE `web_forum_topics`
  ADD CONSTRAINT `web_forum_topics` FOREIGN KEY (`topic_by`) REFERENCES `web_users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `web_forum_topics_ibfk_1` FOREIGN KEY (`topic_cat`) REFERENCES `web_forum_category` (`cat_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `web_forum_topics_ibfk_2` FOREIGN KEY (`topic_by`) REFERENCES `web_users` (`id`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
